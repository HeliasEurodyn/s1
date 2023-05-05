<?php
/**
 * @feed-module		GOF_Skroutz
 * @author-name 	Michail Gkasios
 * @copyright		Copyright (C) 2015 GKASIOS
 * @license			GNU/GPL, see http://www.gnu.org/licenses/old-licenses/gpl-2.0.txt
 */

class ModelFeedGOFSkroutz extends Model
{
	public function loadLanguage($language_id)
	{
		// Language
		$query = $this->db->query("SELECT directory FROM `" . DB_PREFIX . "language` WHERE language_id = '" . $language_id . "'");
		$language_directory = $query->rows[0]['directory'];

		$language = new Language($language_directory);
		$this->registry->set('language', $language);
	}

  	public function createFile($file_path_name, $fetch_rows, $language_id, $color_option_id, $size_option_id)
	{
		$this->load->language('feed/GOF_Skroutz');

		$xml  = '<?xml version="1.0" encoding="utf-8"?>';
		$xml .= '<store name="' . $this->config->get('config_name') . '" url="' . HTTP_SERVER . '" encoding="utf8">';
		$xml .= '<created_at>' . date("Y-m-d h:m") . '</created_at>';
		$xml .= '<products>';

		file_put_contents($file_path_name . '.xml', "\xEF\xBB\xBF" . $xml);

		$xml = '';

		$start_row = 1;
		$end_row = $fetch_rows;
		$products = $this->model_feed_GOF_Skroutz->getProducts($start_row, $end_row, $language_id, $color_option_id, $size_option_id);

		//while($products != null)
		//{
			foreach($products as $product)
			{
				$xml .= $this->productXML($product);
			}

			if(!file_put_contents($file_path_name . '.xml', $xml, FILE_APPEND))
			{
				return false;
			}

			$xml = '';

			//$start_row = $start_row + $fetch_rows;
			//$end_row = $end_row + $fetch_rows;
			//$products = $this->model_feed_GOF_Skroutz->getProducts($start_row, $end_row, $language_id, $color_option_id, $size_option_id);
		//}

		$xml = '</products>';
		$xml .= '</store>';

		if(!file_put_contents($file_path_name . '.xml', $xml, FILE_APPEND))
		{
			return false;
		}

		if(!$this->zipSkroutzXML($file_path_name))
		{
			return false;
		}

		return true;
  	}

	private function productXML($product)
	{
		$xml = '';
		$image = '<image><![CDATA[' . HTTP_SERVER . 'image/' . $product['image'] . ']]></image>';
		$category = '<category><![CDATA[' . $product['category_path'] . ']]></category>';

		/*if($product['manufacturer'] == '')
		{
			$manufacturer = '<manufacturer><![CDATA[' . $product['manufacturer'] . ']]></manufacturer>';
		}
		else
		{
			$manufacturer = '<manufacturer><![CDATA[' . $this->language->get('manufacturer_unknown') . ']]></manufacturer>';
		} */
		
		
		if($product['manufacturer'] == '')
		{
			$manufacturer = '<manufacturer><![CDATA[OEM]]></manufacturer>';
		}
		else
		{
			$manufacturer = '<manufacturer><![CDATA[OEM]]></manufacturer>';
		} 

		if($product['isbn'] != null)
		{
			$isbn = '<isbn>' . $product['isbn'] . '</isbn>';
		}
		else
		{
			$isbn = '';
		}

		 if ($product['mpn'] == '1') 
             {
			   $stock_st = $this->language->get('text_instock20');
             }   
         else if ($product['mpn'] == '2') 
              {
				$stock_st = $this->language->get('text_instock7');
              }   
         else
              {
				$stock_st = $this->language->get('text_instock');
              }   
								
		
		$availability = '<availability>' . $stock_st . '</availability>';

		/* if($product['size_options'] != null)
		{
			$size_options = '<size>' . $product['size_options'] . '</size>';
		}
		else
		{
			$size_options = '';
		} */
		
		$size_options = '';
		
		if($this->config->get('GOF_Skroutz_shipping_cost') != '')
		{
			$shipping = '<shipping type="accurate" currency="euro">' . $this->config->get('GOF_Skroutz_shipping_cost') . '</shipping>';
		}
		else
		{
			$shipping = '';
		}

		$color_options = array_filter(explode(",", $product['color_options']));
		$color_num = count($color_options);

		if($product['color_options'] == null)
		{
			$product_id  = '<product id="' . $product['product_id'] . '">';
			$name = '<name><![CDATA[' . $product['name'] . ' ' . $product['upc'] . ']]></name>';
			$link = '<link><![CDATA[' . HTTP_SERVER . 'index.php?route=product/product&product_id=' . $product['product_id'] . ']]></link>';
			
			//$price_w_vat = $product['price'];

			$price_cus_discount = $product['price'];
			$price_rem = fmod($price_cus_discount,1);
                                
            if($price_rem > 0)
                {
                $price_cus_discount = $price_cus_discount - $price_rem + 1;
                }
          
            $discount = $product['discount'];
            $price_rem =  fmod($discount,1);
            if($price_rem > 0)
               {
               $discount = $discount - $price_rem + 1;
               } 
                             
            $special = $product['special'];
            $price_rem =  fmod($special,1); 
            if($price_rem > 0)
                {
                $special = $special - $price_rem + 1;
                }
			
			$semi_price = ($discount ? $discount : $price_cus_discount);
			$final_price = ($special ? $special : $semi_price);
			
			
			
			$price_with_vat = '<price_with_vat>' . number_format((float)$final_price, 2, '.', '') . '</price_with_vat>';
			$stock = '<stock>' . $product['instock'] . '</stock>';

			/* if($product['mpn'] != null)
			{
				$mpn = '<mpn>' . $product['model'] . '</mpn>';
			}
			else
			{
				$mpn = '<mpn>' . $product['model'] . '</mpn>';
			} */

			if($product['ean'] != null)
			{
				$ean = '<ean>' . $product['ean'] . '</ean>';
			}
			else
			{
				$ean = '<ean>' . $product['product_id'] . '</ean>';
			}

			$xml = $product_id . $name . $link . $image . $category . $price_with_vat . $stock . $availability  . $manufacturer . $isbn . $ean . $size_options . $shipping .  '</product>';
		}
		else
		{
			$color_options = array_filter(explode(",", $product['color_options']));
			$color_options_quantity = explode(",", $product['color_options_quantity']);
			$color_options_price = explode(",", $product['color_options_price']);
			$color_count = count($color_options);

			for($color_num = 0; $color_num  < $color_count; $color_num++)
			{
			
				/*
				if($product['mpn'] != null)
				{
					$mpn = $product['mpn'];
				}
				else
				{
					$mpn = $product['product_id'];
				} */

				if($product['ean'] != null)
				{
					$ean = $product['ean'];
				}
				else
				{
					$ean = $product['product_id'];
				}

				if($color_count > 1)
				{
					$product_id  = '<product id="' . $product['product_id'] . '-' . $color_options[$color_num] . '">';
					$mpn = $mpn . '-' . $color_options[$color_num];
					$ean = $ean . '-' . $color_options[$color_num];
				}
				else
				{
					$product_id  = '<product id="' . $product['product_id'] . '">';
				}

				$mpn = '<mpn>' . $mpn . '</mpn>';
				$ean = '<ean>' . $ean . '</ean>';

				$name = '<name><![CDATA[' . $product['name'] . ' ' . $product['upc'] . ' - ' . $color_options[$color_num] . ']]></name>';
				$link = '<link><![CDATA[' . HTTP_SERVER . 'index.php?route=product/product&product_id=' . $product['product_id'] . '&color=' . $color_options[$color_num] . ']]></link>';

				
				
				$price_cus_discount = $product['price'];
				$price_rem = fmod($price_cus_discount,1);
									
				if($price_rem > 0)
					{
					$price_cus_discount = $price_cus_discount - $price_rem + 1;
					}
			  
			   $discount = $product['discount'];
				$price_rem =  fmod($discount,1);
				if($price_rem > 0)
				   {
				   $discount = $discount - $price_rem + 1;
				   } 
								 
				$special = $product['special'];
				$price_rem =  fmod($special,1); 
				if($price_rem > 0)
					{
					$special = $special - $price_rem + 1;
					}
				
				$semi_price = ($discount ? $discount : $price_cus_discount);
				$final_price = ($special ? $special : $semi_price);
				
				
				
				$price_with_vat = '<price_with_vat>' .  number_format((float)$final_price, 2, '.', '') . '</price_with_vat>';

				//if($product['instock'] == 'N' || $color_options_quantity[$color_num] == 0)
				//{
				//	$product['instock'] = 'N';
				//}

				$stock = '<stock>' . $product['instock'] . '</stock>';

				$color = '<color>' . $color_options[$color_num] . '</color>';

				$xml .= $product_id . $name . $link . $image . $category . $price_with_vat . $stock . $availability . $manufacturer  . $isbn . $ean . $size_options . $shipping . $color .  '</product>';
			}
		}

		return $xml;
	}

	private function zipSkroutzXML($file_path_name)
	{
		$zip = new ZipArchive();

		$filename = $file_path_name . '.zip';

		$opened = $zip->open($filename, ZIPARCHIVE::CREATE | ZIPARCHIVE::OVERWRITE);

		if($opened !== true)
		{
			return false;
		}

		$zip->addFile($file_path_name . '.xml','skroutz.xml');

		$zip->close();

		return true;
	}

	public function sendFile($file_path)
	{
		//@apache_setenv('no-gzip', 1);
		@ini_set('zlib.output_compression', 'Off');

		$path_parts = pathinfo($file_path);
		$file_name  = $path_parts['basename'];
		$file_ext   = $path_parts['extension'];

		if(is_file($file_path))
		{
			$file_size  = filesize($file_path);
			$file = @fopen($file_path,"rb");

			if($file)
			{
				// set the headers, prevent caching
				header("Pragma: public");
				header("Expires: -1");
				header("Cache-Control: public, must-revalidate, post-check=0, pre-check=0");
				header("Content-Disposition: attachment; filename=\"$file_name\"");
				header("Content-Type: application/zip");
				header("Content-Length: $file_size");
				header('Accept-Ranges: bytes');
				set_time_limit(0);
				fseek($file, 0);
				while(!feof($file))
				{
					print(@fread($file, 1024*8));
					ob_flush();
					flush();
					if(connection_status()!=0)
					{
						@fclose($file);
						exit;
					}
				}
				// file save was a success
				@fclose($file);
				exit;
			}
			else
			{
				// file couldn't be opened
				header("HTTP/1.0 500 Internal Server Error");
				exit;
			}
		}
		else
		{
			// file does not exist
			header("HTTP/1.0 404 Not Found");
			exit;
		}
	}

	public function getProducts($from_id = 1, $to_id = 1000000, $language_id = 1, $color_option_id = 1, $size_option_id = 1)
	{
		$nl = "\n";
		$sql = "SELECT	g.product_id," . $nl;
		$sql .= "		g.name," . $nl;
		$sql .= "		g.image," . $nl;
		$sql .= "		g.category_id," . $nl;
		$sql .= "		g.category_name," . $nl;
		$sql .= "		g.price," . $nl;
		$sql .= "		g.model," . $nl;
		
		
		$sql .= "	(	SELECT ";
		$sql .= "		CONCAT_WS(' > ',d3.name, d2.name , d1.name, d.name) ";
	//	$sql .= "		d3.name , ";
	//	$sql .= "		d3.name , ";
	//	$sql .= "		d1.name , ";
	//	$sql .= "		d.name  ";

		$sql .= "		from " . DB_PREFIX . "category c  ";
		$sql .= "		inner join " . DB_PREFIX . "category_description d on c.category_id = d.category_id and d.language_id = " . $language_id ." ";

		$sql .= "		left outer join " . DB_PREFIX . "category c1 on c.parent_id = c1.category_id ";
		$sql .= "		left outer join " . DB_PREFIX . "category_description d1 on c1.category_id = d1.category_id and d1.language_id = " . $language_id ." ";

		$sql .= "		left outer join " . DB_PREFIX . "category c2 on c1.parent_id = c2.category_id ";
		$sql .= "		left outer join " . DB_PREFIX . "category_description d2 on c2.category_id = d2.category_id and d2.language_id = " . $language_id ." ";

		$sql .= "		left outer join " . DB_PREFIX . "category c3 on c2.parent_id = c3.category_id  ";
		$sql .= "		left outer join " . DB_PREFIX . "category_description d3 on c3.category_id = d3.category_id and d3.language_id = " . $language_id ." ";
		
        $sql .= "		WHERE c.category_id = g.category_id ";
		
		$sql .= " ) AS category_path , ";
		
		$sql .= " (SELECT price FROM " . DB_PREFIX . "product_special ps WHERE "
                . " ps.product_id = g.product_id "
                        . " AND ((ps.date_start = '0000-00-00' OR ps.date_start < NOW()) "
                        . " AND (ps.date_end = '0000-00-00' OR ps.date_end > NOW())) "
                        . " ORDER BY ps.priority ASC, ps.price ASC LIMIT 1) AS special, " ;
		
		$sql .=  " (SELECT price FROM " . DB_PREFIX . "product_discount pd2 "
                        . " WHERE pd2.product_id = g.product_id "
                        . " AND pd2.customer_group_id = '" . (int)$customer_group_id . "' "
                        . " AND pd2.quantity = '1' AND "
                        . " ((pd2.date_start = '0000-00-00' OR pd2.date_start < NOW()) "
                        . " AND (pd2.date_end = '0000-00-00' OR pd2.date_end > NOW())) "
                        . " ORDER BY pd2.priority ASC, pd2.price ASC LIMIT 1) AS discount, " ;
		
		$sql .= "		g.instock," . $nl;
		$sql .= "		g.stock_status_id," . $nl;
		$sql .= "		g.manufacturer," . $nl;
		$sql .= "		g.mpn," . $nl;
		$sql .= "		g.upc," . $nl;
		$sql .= "		g.isbn," . $nl;
		$sql .= "		g.ean," . $nl;
		$sql .= "		GROUP_CONCAT(g.size_options) AS `size_options`," . $nl;
		$sql .= "		GROUP_CONCAT(g.size_options_quantity) AS `size_options_quantity`," . $nl;
		$sql .= "		GROUP_CONCAT(g.size_options_price) AS `size_options_price`," . $nl;
		$sql .= "		GROUP_CONCAT(g.color_options) AS `color_options`," . $nl;
		$sql .= "		GROUP_CONCAT(g.color_options_quantity) AS `color_options_quantity`," . $nl;
		$sql .= "		GROUP_CONCAT(g.color_options_price) AS `color_options_price`" . $nl;
		$sql .= "FROM" . $nl;
		$sql .= "(" . $nl;
		$sql .= "	SELECT 	p.product_id AS `product_id`," . $nl;
		$sql .= "			pd.name AS `name`," . $nl;
		$sql .= "			p.image AS `image`," . $nl;
		$sql .= "			p.model AS `model`," . $nl;
		$sql .= "			c.category_id AS `category_id`," . $nl;
		$sql .= "			c.category_name AS `category_name`," . $nl;
		$sql .= "			p.price AS `price`," . $nl;
		$sql .= "			IF(p.quantity = 0, 'N','Y') AS `instock`," . $nl;
		$sql .= "			p.stock_status_id AS `stock_status_id`," . $nl;
		$sql .= "			m.name AS `manufacturer`," . $nl;
		$sql .= "			p.mpn AS `mpn`," . $nl;
		$sql .= "			p.upc AS `upc`," . $nl;
		$sql .= "			p.isbn AS `isbn`," . $nl;
		$sql .= "			p.ean AS `ean`," . $nl;
		$sql .= "			GROUP_CONCAT(ovd.name SEPARATOR ',') AS `size_options`," . $nl;
		$sql .= "			GROUP_CONCAT(pov.quantity SEPARATOR ',') AS `size_options_quantity`," . $nl;
		$sql .= "			GROUP_CONCAT(CONCAT(pov.price_prefix, '', pov.price) SEPARATOR ',') AS `size_options_price`," . $nl;
		$sql .= "			GROUP_CONCAT(ovd.name SEPARATOR ',') AS `color_options`," . $nl;
		$sql .= "			GROUP_CONCAT(pov.quantity SEPARATOR ',') AS `color_options_quantity`," . $nl;
		$sql .= "			GROUP_CONCAT(CONCAT(pov.price_prefix, '', pov.price) SEPARATOR ',') AS `color_options_price`" . $nl;
		$sql .= "	FROM `" . DB_PREFIX . "product` AS `p`" . $nl;
		$sql .= "	LEFT JOIN `" . DB_PREFIX . "product_description` AS `pd`" . $nl;
		$sql .= "		ON p.product_id = pd.product_id" . $nl;
		$sql .= "	LEFT JOIN `" . DB_PREFIX . "product_option_value` AS `pov`" . $nl;
		$sql .= "		ON pov.product_id = p.product_id" . $nl;
		$sql .= "	LEFT JOIN `" . DB_PREFIX . "option_value_description` AS `ovd`" . $nl;
		$sql .= "		ON ovd.option_value_id = pov.option_value_id" . $nl;
		$sql .= "	LEFT JOIN `" . DB_PREFIX . "manufacturer` AS `m`" . $nl;
		$sql .= "		ON p.manufacturer_id = m.manufacturer_id" . $nl;
		$sql .= "	LEFT JOIN" . $nl;
		$sql .= " 	(" . $nl;
		$sql .= "		SELECT 	ptc.product_id AS `product_id`," . $nl;
		$sql .= "				cd.category_id AS `category_id`," . $nl;
		$sql .= "				cd.name AS `category_name`" . $nl;
		$sql .= "		FROM " . DB_PREFIX . "product_to_category AS `ptc`" . $nl;
		$sql .= "		LEFT JOIN `" . DB_PREFIX . "category_description` AS `cd`" . $nl;
		$sql .= "			ON ptc.category_id=cd.category_id" . $nl;
		$sql .= "		WHERE 	cd.language_id = " . $language_id . $nl;
	//	$sql .= "			AND ptc.product_id >= " . $from_id . $nl;
	//	$sql .= "			AND ptc.product_id <= " . $to_id . $nl;
       //        $sql .= "			AND ptc.skroutz = 1 ". $nl;
		$sql .= "		GROUP BY ptc.product_id" . $nl;
		$sql .= "	) AS `c`" . $nl;
		$sql .= "		ON p.product_id=c.product_id" . $nl;
		$sql .= "	WHERE 	pd.language_id = " . $language_id . $nl;
	//	$sql .= "		AND p.product_id >= " . $from_id . $nl;
	//	$sql .= "		AND p.product_id <= " . $to_id . $nl;
                $sql .= "		AND p.skroutz = 1 ". $nl;
				 $sql .= "		AND p.quantity > 5 ". $nl;
				  $sql .= "		AND p.status = 1 ". $nl;
		$sql .= "		AND (ovd.language_id = " . $language_id ." OR ovd.language_id IS NULL)" . $nl;
		$sql .= "		AND DATE(NOW()) > DATE(p.date_available)" . $nl;
	//	$sql .= "		AND	(pov.option_id = " . $size_option_id . " OR pov.option_id = " . $color_option_id . " OR pov.option_id IS NULL)" . $nl;
		$sql .= "	GROUP BY p.product_id,pov.option_id" . $nl;
		$sql .= ") AS `g`" . $nl;
		$sql .= "GROUP BY g.product_id" . $nl;

                
             //   echo $sql; 
                
		$query = $this->db->query($sql);

		return $query->rows;
	}
}
?>